all: build

build:
	xbuild /nologo /verbosity:quiet

test:
	nunit-console Test/bin/Debug/Test.dll

.PHONY: build test
