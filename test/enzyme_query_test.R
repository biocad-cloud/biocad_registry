require(biocad_registry);

let biocad_registry = open_registry("root", 123456);
let enzyme = biocad_registry |> enzyme("1.3.1.-");

print(enzyme);
print(attr(enzyme,"sql"));