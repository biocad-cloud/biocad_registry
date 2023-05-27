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

        const operons = gene_text |> content(plain_text = TRUE) |> read.operon();
        const motifs = motif_text |> content(plain_text = TRUE) |> read.motifs();

        # str(operons);

        # for(operon in operons) {
        #     let operon_genes = lapply([operon]::members, x -> as.list(x));
        #     operon_genes = lapply(operon_genes, function(x) {
        #         x$id = x$vimssId;
        #         x$dblinks = list(MicrobesOnline = x$vimssId);
        #         x;
        #     }, names = x -> x$locusId);
        #     str(operon_genes);

        #     biocad_registry::put.operon(genome$id, operon_genes);
        # }

        for(motif in motifs) {
            let sites = lapply([motif]::regulatorySites, x -> as.list(x));
            let type = [motif]::type;
            let family = [motif]::family;
            let regulator = as.list([motif]::regulator);

            str(as.character(type));
            str(family);
            str(regulator);

            str(sites);
            # str(motif);
    # stop();

            biocad_registry::put.regulation(
                genome_id = genome$id, regulator, family, type, sites);

            stop();
        }

       #  stop();
       # sleep(3);
    }
}