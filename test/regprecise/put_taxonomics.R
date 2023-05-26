imports "package_utils" from "devkit";

package_utils::attach(`${@dir}/../../`);

setwd(@dir);

require(JSON);

let data = "taxonomics_group.json"
|> readText()
|> json_decode()
;

# str(data);

for(tax in data) {
    str(tax);
    tax_id = last(strsplit(tax$id, "=", fixed = TRUE));
    tax_name = tax$name;
    str(tax_id);

    biocad_registry::put.taxonomic_group(tax_name, tax_id);
}