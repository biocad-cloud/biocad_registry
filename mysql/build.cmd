REM @echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set R_src="../src/biocad_storage/mysql"

%reflector% --reflects /sql ./cad_registry20241221.sql -o %R_src% /namespace biocad_registryModel --language visualbasic /split /auto_increment.disable

REM pause