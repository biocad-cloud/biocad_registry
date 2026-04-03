require(biocad_registry);

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");
let rep = "K:\diamonds";
let mysql = "C:\\Program Files\\MySQL\\MySQL Workbench 8.0 CE\\mysql.exe";
let cmd = biocad_registry |> imports_sql(rep, mysql,batch_script = TRUE);

writeLines(cmd, con = file.path( rep, "import.cmd") );