imports "package_utils" from "devkit";

package_utils::attach(`${@dir}/../../`);

setwd(@dir);

require(JSON);

let data = "taxonomics_group.json"
|> readText()
|> json_decode()
;

str(data);