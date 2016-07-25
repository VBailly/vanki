#!/bin/bash


function play {

	while true
	do
		mono Vanki.exe -n
		read answer
		mono Vanki.exe -a "$answer"
		echo "-------------------------------------"
	done
}

function add {
	while true
	do
		echo "Type the question"
		read question
		echo "Type the answer"
		read answer
		mono Vanki.exe -q "$question" -a "$answer"
		echo "-------------------------------------"
	done
}

if [ ${1} == "play" ]
then
	play
elif [ ${1} == "add" ]
then
	add
else
	echo "You much type either play or add"
fi