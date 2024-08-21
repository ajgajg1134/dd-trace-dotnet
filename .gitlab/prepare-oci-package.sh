#!/bin/bash

set -eo pipefail

PACKAGE_VERSION=$(< ../artifacts/version.txt)

if [ -n "$CI_COMMIT_TAG" ] || [ -n "$DOTNET_PACKAGE_VERSION" ]; then
  VERSION=$PACKAGE_VERSION
else
  VERSION=$PACKAGE_VERSION-${CI_VERSION_SUFFIX:1}
fi

# https://github.com/har7an/bash-semver-regex
SEMVER_REGEX="^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(-((0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*)(\.(0|[1-9][0-9]*|[0-9]*[a-zA-Z-][0-9a-zA-Z-]*))*))?(\+([0-9a-zA-Z-]+(\.[0-9a-zA-Z-]+)*))?$"

if ! [[ $VERSION =~ $SEMVER_REGEX ]]; then
  echo "Invalid version format: '$VERSION' (check https://semver.org/ for more information)"
  exit 1
fi

echo "'VERSION=$VERSION'"

if [ -z "$ARCH" ]; then
  ARCH=amd64
fi

if [ "$ARCH" == "amd64" ]; then
  SUFFIX=""
elif [ "$ARCH" == "arm64" ]; then
  SUFFIX=".arm64"
else
  echo "Unsupported architecture: $ARCH"
  exit 1
fi

SRC_TAR="../artifacts/datadog-dotnet-apm-$PACKAGE_VERSION${SUFFIX}.tar.gz"

if [ ! -f $SRC_TAR ]; then
   echo "$SRC_TAR was not found!"
   exit 1
fi

mkdir -p sources

# extract the tarball, making sure to preserve the owner and permissions
tar --same-owner -pxvzf $SRC_TAR -C sources

echo -n $VERSION > sources/version
