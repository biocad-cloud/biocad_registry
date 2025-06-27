// export R# package module type define for javascript/typescript language
//
//    imports "data_exports" from "biocad_registry";
//
// ref=biocadRegistry.exports_api@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace data_exports {
   /**
     * @param wrap_tqdm default value Is ``true``.
   */
   function export_by_cids(registry: object, cid: any, wrap_tqdm?: boolean): object;
   /**
   */
   function export_fingerprints(registry: object): any;
   /**
     * @param page_size default value Is ``10000``.
   */
   function export_metabolites(registry: object, page_size?: object): any;
}
