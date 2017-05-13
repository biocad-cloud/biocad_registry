<!DOCTYPE html>
<html>
<head>
    <%= ./includes/head.vbhtml %>
    <?vb $title = "Data Analysis Tools" ?>
    <?vb $active2 = "active" ?>
</head>
<body>

    <div id="main-wrapper">

        <%= ./includes/navigation-bar.vbhtml %>

        <div class="row">
            <div class="small-12 columns">
                <h1>Biostack platform</h1>
                <h3>Navigate reference database</h3>
                <ul>
                    <li><a href="/Application/COG_myva/index.vbhtml"><%= @COG_myva %></a></li>
                    <li><a href="./Uniprot.vbhtml"><%= @Uniprot %></a></li>
                    <li><a href="./RegPrecise.vbhtml"><%= @RegPrecise %></a></li>
                </ul>
            </div>
        </div>

        <%= ./includes/footer.vbhtml %>

    </div>

</body>
</html>
