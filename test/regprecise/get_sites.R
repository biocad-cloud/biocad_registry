imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../../`);

setwd(@dir);

require(JSON);

const alldata = biocad_registry::pull_taxonomic();
const local = http.cache("./cache/");

for(tax in alldata) {
    const genomes = biocad_registry::pull_taxonomic(tax$id);

    # str(genomes);

    for(genome in genomes) {
        str(genome);

        const gene_text = requests.get(`https://regprecise.lbl.gov/ExportServlet?type=gene&genomeId=${genome$id}`, cache = local);
        const motif_text = requests.get(`https://regprecise.lbl.gov/ExportServlet?type=site&genomeId=${genome$id}`, cache = local);

       # sleep(3);
    }
}