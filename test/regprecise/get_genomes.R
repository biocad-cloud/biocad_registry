imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../../`);

setwd(@dir);

require(JSON);

const alldata = biocad_registry::pull_taxonomic();
const local = http.cache("./cache/");

# str(alldata);

for(tax in alldata) {
    str(tax);

    const url = `https://regprecise.lbl.gov/collection_tax.jsp?collection_id=${tax$id}`;
    const html = jQuery::load(url, proxy = local);

    let tbl = html[".stattbl"];
    let body = tbl["tbody"];
    let rows = body["tr"];
    let taxonomics = lapply(rows, function(r) {
        let cells = r["td"];
        let name = cells[1];
        
        name = name["innerHTML"];
        
        let id = Html::link(name);
        
        name = Html::plainText(name);
        
        {
            id: id, 
            name: name
        }
    }) |> which(x -> startsWith(x$id, "genome.jsp"));

    # str(taxonomics);

    for(genome in taxonomics) {
        str(genome);
    }

    stop();
}