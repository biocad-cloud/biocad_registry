// export R# source type define for javascript/typescript language
//
// package_source=biocad_registry

declare namespace biocad_registry {
   module _ {
      /**
      */
      function onLoad(): object;
   }
   /**
     * @param host default value Is ``localhost``.
     * @param port default value Is ``3306``.
     * @param dbname default value Is ``cad_registry``.
   */
   function open_registry(user: any, passwd: any, host?: any, port?: any, dbname?: any): object;
}
