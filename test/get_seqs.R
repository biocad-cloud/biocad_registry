imports "package_utils" from "devkit";
imports ["jQuery", "Html", "http"] from "webKit";

package_utils::attach(`${@dir}/../`);

setwd(@dir);

const local = http.cache("./.cache/");

parse_seqs = function(id) {
    const url = `http://www.microbesonline.org/cgi-bin/fetchLocus.cgi?locus=${id}&disp=4`;
    const html = requests.get(url, cache = local) |> http::content(plain_text = TRUE);

    # print(html);

    const tables = Html::tables(html);
    const title = Html::title(html);

    print(title);
    print(tables);

    stop();
}


str(parse_seqs(id = 5519438));