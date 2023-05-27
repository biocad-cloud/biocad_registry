imports "http" from "WebKit";

const url = "http://registry.biocad.cloud:8848/err500_test/";
const data = requests.post(url);
const result = data |> http::content(data);

str(result);
