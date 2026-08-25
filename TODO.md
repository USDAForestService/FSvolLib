
## Remove non-necessary platform dependencies
 - remove `dllmain.cpp` - it currently doesn't do anything. 
 - remove `windows.h` and `framework.h` 

 ## Make PCH optional and default to off
Right now PCH use is inconsistent. It doesn't look like it is included in all header files.
Part of this may be due to build times not takeing long enough for it to be necessary, and the only 
headers that are being pre-compiled are the windows.h header which aren't being used.
In the near term lets remove the need 