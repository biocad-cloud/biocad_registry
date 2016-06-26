#!/usr/bin/perl

use strict;
use warnings;
use File::Basename;
use File::Spec;

# bbh between query fasta and subjects in a directory.

my $query  = $ARGV[0];
my $sbjDIR = $ARGV[1];
my $outDIR = $ARGV[2];
my $qn     = basename($query);

mkdir $outDIR;
print "bbh out directory is $outDIR...\n\n";

my $args = "./makeblastdb -in \"$query\" -dbtype prot"; 
system($args);

# for each file in subject directory
#
opendir (DIR, $sbjDIR) or die $!;

while (my $sbjFa = readdir(DIR)) {

	print "  ==> $sbjFa\n";
		
	if ($sbjFa eq  ".") {
		next;
	}
	if ($sbjFa eq "..") {
		next;
	}

	my $qvs = "$outDIR/$qn._vs__$sbjFa.txt";
	my $svq = "$outDIR/$sbjFa._vs__$qn.txt";
	my $sbj = "$sbjDIR/$sbjFa";	

	print "qvs -> $qvs\n";
	print "svq -> $svq\n";
	print "subject -> $sbj\n";

	$args = "./makeblastdb -in \"$sbj\" -dbtype prot";
	print "$args\n";
	system($args);

	$qvs = "./blastp -query \"$query\" -db \"$sbj\" -out \"$qvs\" &";
	$svq = "./blastp -query \"$sbj\" -db \"$query\" -out \"$svq\" &";

	print "$qvs\n";
	print "$svq\n";

	system($qvs);
	system($svq);

}

closedir(DIR);
