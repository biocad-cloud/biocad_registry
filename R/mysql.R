#' open mysqli connection to biocad_registry
#' 
const open_registry = function(user, passwd, host = "localhost", port = 3306, dbname = "cad_registry") {
    mysql::open(
        user_name = user,
        password = passwd,
        dbname = dbname,
        host  = host,
        port = port
    );
}