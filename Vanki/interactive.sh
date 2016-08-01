#!/bin/bash


function play {

	while true
	do
		clear
		next=$(mono Vanki.exe -n)
		echo $next
		read answer
		while [[ $next == *"Come back at"* ]] 
		do
			next=$(mono Vanki.exe -n)
			clear
			echo $next
			read answer
		done
		while [[ -z "$answer" ]]
		do
			mono Vanki.exe --clue
			read answer
		done
		mono Vanki.exe -a "$answer"
		read answer
		clear
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
