#!/usr/bin/perl

my $pwmDir  = "/media/xieguigang/_media_DataStore/GCModeller/Regprecise/MEME/pwm/";
my $outFile = "./Regprecise.fasta";


# Opening the directory
opendir (faDIR, $pwmDir) or die $!;
open (OUT,">>$outFile") || die "cannot open file $!";

# Reading the directory
while (my $fasta = readdir(faDIR)) {

    if ($fasta eq ".")  {
        next; 
    }
    if ($fasta eq "..") {
        next; 
    }   

    $fasta = "$pwmDir/$fasta";

    print "$fasta\n"; 
    open (IN, "$fasta") || die "cannot open file $!";
    

    while ( <IN> )
    {
        chomp $_;
        print OUT"$_\n";

    }
    print OUT"\n";

    close (IN);
}
close (faDIR);
close (OUT);
