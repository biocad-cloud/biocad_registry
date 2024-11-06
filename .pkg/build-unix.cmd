REM @echo off

SET drive=%~d0
SET R_HOME=%drive%\GCModeller\src\R-sharp\App\net8.0
SET Rscript="%R_HOME%/Rscript.exe"
SET REnv="%R_HOME%/R#.exe"

%Rscript% --build /src ../ /save ../biocad_registry.zip --skip-src-build
%REnv% --install.packages ../biocad_registry.zip

pause