#!/bin/bash

while true
do
	mono Vanki.exe -n
	read answer
	mono Vanki.exe -a "$answer"
done
