require(biocad_registry);
require(LLMs);

imports "registry" from "biocad_registry";
imports "ollama" from "Agent";

let biocad_registry = open_registry("xieguigang", 123456, host ="192.168.3.15");

setup_global_hook(ollama::new(
    model = "qwen3:30b", 
    ollama_server = "127.0.0.1:11434",
    preserve_memory = FALSE
));

ontology_translation(biocad_registry, ontology = "RefMet");