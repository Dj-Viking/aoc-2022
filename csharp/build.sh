set -e

if [ -z "$1" ]
  then
    echo "✨ Please provide a number as the first argument of this script ✨";
    echo "e.g. sh build.sh 5";
    exit 1;
fi

DAY="$1";
cd "day$DAY";
dotnet build;
cd ..;