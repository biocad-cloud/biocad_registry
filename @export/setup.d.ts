// export R# package module type define for javascript/typescript language
//
//    imports "setup" from "biocad_registry";
//
// ref=biocadRegistry.setup@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace setup {
   /**
     * @param env default value Is ``null``.
   */
   function setup_metabolites(registry: object, kegg: any, refmet: any, env?: object): any;
}
