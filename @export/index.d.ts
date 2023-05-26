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
     * @param id default value Is ``null``.
   */
   function pull_taxonomic(id?: any): object;
   module put {
      /**
      */
      function genome_group(grp: any, genomes: any): object;
      /**
        * @param note default value Is ````.
      */
      function taxonomic_group(name: any, id: any, note?: string): object;
   }
}
