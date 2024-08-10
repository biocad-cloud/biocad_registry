#' open mysqli connection to biocad_registry
#' 
#' @param host the mysql server host
#' @param dbname the database name of the target mysql database, 
#'    value may be ``cad_registry`` or ``biocad_registry``.
#' 
const open_registry = function(user, passwd, 
                               host = "localhost", 
                               port = 3306, 
                               dbname = "cad_registry") {
    mysql::open(
        user_name = user,
        password = passwd,
        dbname = dbname,
        host  = host,
        port = port
    );
}