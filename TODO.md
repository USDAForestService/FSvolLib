
## .NET P/Invoke interop (FSvolLibInterop DLL)

The CMake option `BUILD_INTEROP=ON` wires in `src/FSvolLib.interop/` as a SHARED DLL target that
links the STATIC core and exposes an `extern "C"` surface for .NET P/Invoke.

### Create `src/FSvolLib.interop/`

New directory containing:
- `CMakeLists.txt` — `add_library(FSvolLibInterop SHARED ...)`, links `FSvolLib`
- `VolumeLibrary.interop.h` — declares `extern "C"` exported functions using only C-safe POD types
- `VolumeLibrary.interop.cpp` — implements the wrappers, converts to/from native C++ models, and delegates to `VolumeLibrary::getInstance()`
- `export.h` — cross-platform export macro:
  ```cpp
  #if defined(_WIN32)
  #  define FSVOLLIB_INTEROP_API __declspec(dllexport)
  #else
  #  define FSVOLLIB_INTEROP_API __attribute__((visibility("default")))
  #endif
  ```

### C interface design: separate interop-facing PODs

We should not expose the internal C++ models directly over the C ABI. The core library should remain C++-native, and the interop layer should define a dedicated set of C-safe structs with a `_C` suffix.

#### Input PODs created by the C caller
- `TreeMeasurment_C`
- `MerchRules_C`
- `VolumeCalculationOptions_C`

These are plain data structs created and owned by the caller. They are converted into the internal C++ model types inside the interop layer before calling the library.

#### Output PODs created and owned by the interop DLL
- `TreeOutput_C`
- `LogOutput_C`
- `BiomassOutput_C`
- `ErrorInfo_C`

These are the only objects returned across the C boundary. They are allocated by the interop DLL and must be freed via exported cleanup functions. The C caller must not touch or delete native C++ objects directly.

### Ownership and lifetime model

- `TreeOutput_C.logs` is a pointer to a `LogOutput_C[]` array allocated by the interop DLL.
- `TreeOutput_C.log_count` records the number of entries in the array.
- `ErrorInfo_C.errorMessage` is a heap-allocated C string owned by the interop DLL.
- The caller frees these using exported functions such as:
  - `void free_tree_output_c(TreeOutput_C* value);`
  - `void free_error_info_c(ErrorInfo_C* value);`
- No `std::string`, `std::vector`, or native C++ object layout should cross the DLL boundary.

### Recommended output struct shapes

```cpp
typedef struct BiomassOutput_C
{
    double grossBoardFoot;
    double grossCubicFoot;
    double dryWeight;
    double greenWeight;
} BiomassOutput_C;

typedef struct LogOutput_C
{
    int logNumber;
    int product;
    int isSecondary;

    double smallEndDiameterActual;
    double largeEndDiameterActual;
    double smallEndDiameterScaled;
    double largeEndDiameterScaled;

    double length;
    double heightToLargeEndDiameter;

    double grossBoardFoot;
    double grossCubicFoot;
    double internationalBoardFoot;

    double greenWeight;
    double dryWeight;
} LogOutput_C;

typedef struct TreeOutput_C
{
    double grossBoardFootPrimary;
    double grossBoardFootSecondary;
    double grossCubicFootPrimary;
    double grossCubicFootSecondary;
    double grossInternationalBoardFoot;

    double totalCubicFoot;
    double stumpCubicFoot;
    double tipCubicFoot;

    double cordMerchantable;
    double greenWeightPrimary;
    double greenWeightSecondary;
    double dryWeightPrimary;
    double dryWeightSecondary;

    BiomassOutput_C greenBio;
    BiomassOutput_C dryBio;

    double carbonContent;

    LogOutput_C* logs;
    size_t log_count;

    int numberOfLogs;
    int errflag;
} TreeOutput_C;

typedef struct ErrorInfo_C
{
    int errorCode;
    char* errorMessage;
} ErrorInfo_C;
```

### C API signatures for `VolumeLibrary.interop.h`

```cpp
bool TryCalculateVolume(const VolumeCalculationOptions_C* options,
                        const TreeMeasurment_C* tree,
                        TreeOutput_C** out,
                        ErrorInfo_C** error);

bool CalculateVolumeWithMerchRules(const VolumeCalculationOptions_C* options,
                                  const TreeMeasurment_C* tree,
                                  const MerchRules_C* merchRules,
                                  TreeOutput_C** out,
                                  ErrorInfo_C** error);

void GetVolumeEquationNumber(const VolumeCalculationOptions_C* options,
                            char* outBuffer,
                            int bufferLen);

double GetHeightAtDiameter(const char* volEqNumber,
                           const TreeMeasurment_C* tree,
                           double diameter);

double GetDiameterAtHeight(const char* volEqNumber,
                           const TreeMeasurment_C* tree,
                           double height);

int GetNumberOfLogs(const VolumeCalculationOptions_C* options,
                    const TreeMeasurment_C* tree);

void GetVersion(char* outBuffer, int bufferLen);
```

### Interop conversion strategy

Each exported wrapper function should:
1. Accept only C-safe POD inputs from the caller.
2. Convert those inputs to the internal C++ model types used by `VolumeLibrary`.
3. Call the appropriate C++ library method.
4. Convert the result to a `*_C` output object allocated by the interop DLL.
5. Return status and an optional `ErrorInfo_C*`.

This creates a clean boundary of concern:
- core library = C++ implementation and calculations
- interop DLL = ABI conversion, allocation, lifetime management
- .NET caller = C/P/Invoke marshalling and consumption

### Final recommendation

Use the separate PODO design for the interop layer rather than trying to make the internal C++ models semi-opaque. This is the safer, cleaner, and more maintainable path for cross-platform .NET interop.

