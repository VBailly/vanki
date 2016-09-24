all: build

build:
	xbuild /nologo /verbosity:quiet

test:
	nunit-console Test/bin/Debug/Test.dll

clean:
	xbuild /target:clean /nologo /verbosity:quiet

.PHONY: build test
