imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../`);

setwd(@dir);

const local = http.cache("E:/UniProt/.cache");

parse_seqs = function(id) {
    const url = `http://www.microbesonline.org/cgi-bin/fetchLocus.cgi?locus=${id}&disp=4`;
    const html = requests.get(url, cache = local) |> http::content(plain_text = TRUE);

    # print(html);

    const tables = Html::tables(html, del_newline = FALSE);
    const title = Html::title(html);
    const seqs = t(last(tables));

    colnames(seqs) = unlist(seqs[1,, drop = TRUE ]);

    print(title);
    # print(seqs);

    print(colnames(seqs));

    const protein = last( seqs$Protein);
    const gene_seq = last( seqs$Coding);

    print(protein);
    print(gene_seq);

    {
        vimssid: id,
        note: title,
        protein: protein,
        gene: gene_seq
    }
}


str(parse_seqs(id = 5519438));

for(i in 1:1000) {
    let db_links = biocad_registry::get_dblinks("MicrobesOnline", page = i);

    # str(db_links);

    if (as.integer(db_links$count) == 0) {
        print("query data complete!");
        break;
    } else {
        for(gene in db_links$query) {
            str(gene);

            let seq = parse_seqs(id = gene$xref_id);
            str(seq);

            sleep(1);
        }
    }
}