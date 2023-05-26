imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../../`);

setwd(@dir);

require(JSON);

const alldata = biocad_registry::pull_taxonomic();
const local = http::cache("./cache/");

# str(alldata);

for(tax in alldata) {
    str(tax);

    const url = `https://regprecise.lbl.gov/collection_tax.jsp?collection_id=${tax$id}`;
    const html = jQuery::load(url, proxy = local);

    let tbl = html[".stattbl"];
    let body = tbl["tbody"];
    let rows = body["tr"];
    let taxonomics = lapply(rows, function(r) {
        var cells = r["td"];
        var name = cells[1];
        
        name = name.innerHTML;
        
        var id = Html.link(name);
        
        name = Html.plainText(name);
        
        return {
            id: id, 
            name: name
        }
    });

    str(group);
    str(taxonomics);

    stop();
}