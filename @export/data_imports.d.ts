// export R# package module type define for javascript/typescript language
//
//    imports "data_imports" from "biocad_registry";
//
// ref=biocad_registry.imports_api@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace data_imports {
   /**
   */
   function genbank_repo(dir: string): object;
   /**
   */
   function imports_genbank(registry: object, genbank: object): any;
   /**
   */
   function imports_genomics(registry: object, genbank: object): any;
   /**
   */
   function imports_taxonomy(registry: object, taxdump: object): any;
}
