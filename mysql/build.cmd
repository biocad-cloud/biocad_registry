@echo off

set reflector="\graphQL\src\mysqli\App\Reflector.exe"
set R_src="../src/registry_data/mysql/mysql"
set sql="./cad_registry-v2-20260101.sql"

CALL %reflector% --reflects /sql %sql% -o %R_src% /namespace biocad_registryModel --language visualbasic /split /auto_increment.disable

REM pause