all: build

nuget:
	nuget restore

build: nuget
	xbuild /nologo /verbosity:quiet

test: build
	nunit-console Test/bin/Debug/Test.dll

clean:
	xbuild /target:clean /nologo /verbosity:quiet

.PHONY: build test
