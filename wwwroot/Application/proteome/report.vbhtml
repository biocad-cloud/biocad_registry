<!DOCTYPE html>
<html>
<head>
    <%= ../../includes/head.vbhtml %>
    <%= ../../includes/breadcrumb/styles.vbhtml %>

    <?vb $title = "Proteome Data Analysis Report" ?>
    <?vb $active2 = "active" ?>
    <?vb $appname = "Proteome Data Analysis Report" ?>
</head>
<body>

    <div id="main-wrapper">

        <%= ../../includes/navigation-bar.vbhtml %>

        <div class="row">
            <div class="small-12 columns">
                <h1>COG myva annotation report</h1>
				<p>COG, namely <strong>Clusters of Orthologous groups of Proteins</strong>. The proteins that make up each COG catagory are assumed to come from an common ancestor protein, and these proteins either can be Orthologs or Paralogs. 
				<li><strong>Orthologs</strong> is the protein derived from the vertical family (species formation) evolved from different species and typically retains the same function as the original protein.</li>
				<li><strong>Paralogs</strong> are those proteins that derive from gene replication in certain species and may evolve new functions related to the original.</li>
				</p>
				<h2>1. Analysis Processes</h2>
				<h4>How did cog build up?</h4>
<p>
Cog is determined by comparing the encoding proteins of all sequenced genomes one by one. When considering proteins from a given genome, this comparison gives one of the most similar proteins in each of the other genomes (hence the need for a complete genome to define cog. Note 1 Every A of these genes are considered in turns. If a mutual best match between these proteins (or subsets) is found, then those best bets will form a cog (note 2). In this way, a member of a cog will be more similar to the other members of the cog than the other proteins in the genome compared, although if absolute similarity is compared. The use of the best matching principle, without the restriction of the artificially selected statistical excision, takes into account the slow evolution and fast evolution of proteins. However, one additional limitation is that a cog must contain one protein from a genome that takes place far from the 3 species.
</p>
<h4>COG annotation Function</h4>
<p><li>1. functional annotation of unknown sequence by known protein;</li>
<li>2. The existence and absence of the Protein number corresponding to the specified cog number to derive the existence of specific metabolic pathways;</li>
<li>3. Each cog number is a class of proteins, and the sequence of the query sequence and the Proteins of the cog number on the alignment are compared, which can determine the conservative site and analyze its evolutionary relation.</li>
</p>
				<img src="./COG_myva.png"></img>
				<h2>2. Analysis Result</h2>	
				<h2>3. References</h2>
				<ol>
				<li>Tatusov et al. (1997).  A genomic perspective on protein families.  Science 278: 631-637.</li>
<li>Koonin et al. (1998).  Beyond complete genomes: from sequence to structure and function.  Curr. Opin. Struct. Biol. 8: 355-363.</li>
<li>Galperin et al. (1999).  Comparing microbial genomes: How the gene set determines the lifestyle.  In Organization of the Prokaryotic Genome,  R.L. Charlebois, Ed. (American Society of Microbiology, Washington, DC) pp. 91-108.</li>
<li>Tatusov et al. (2000).  A genomic perspective on protein families.  Nucleic Acids Res. 28: 33-6.</li>
</ol>
                <%= ../../includes/breadcrumb/applications.vbhtml %>
            </div>
        </div>

        <%= ../../includes/footer.vbhtml %>

    </div>
</body>
</html>
