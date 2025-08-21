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
     * @param file default value Is ``null``.
     * @param page_size default value Is ``10000``.
     * @param env default value Is ``null``.
   */
   function export_metabolites(registry: object, file?: any, page_size?: object, env?: object): any;
   /**
   */
   function export_topic_structdata(registry: object, tag_name: string): object;
}
