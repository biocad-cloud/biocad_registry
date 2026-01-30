// export R# package module type define for javascript/typescript language
//
//    imports "exports" from "biocad_registry";
//
// ref=biocadRegistry.exports@biocad_registry, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace exports {
   /**
    * Export smiles table for run molecule strucutre analysis
    * 
    * 
     * @param registry -
     * @param dbname -
   */
   function export_smiles_data(registry: object, dbname: string): any;
   /**
     * @param dbname default value Is ``null``.
   */
   function metabolite_table(registry: object, dbname?: string): any;
   /**
    * Make exports of the component models for run virtual cell annotations
    * 
    * 
     * @param registry -
     * @param repo -
   */
   function virtualcell_componentdb(registry: object, repo: string): any;
}
