set -e

if [ -z "$1" ]
  then
    echo "✨ Please provide a number as the first argument of this script ✨";
    echo "e.g. sh newDay.sh 5";
    exit 1;
fi

DAY="$1"
FOLDER="Day$DAY"

echo "creating folder for day $DAY..."
mkdir "$FOLDER";
echo ""
echo "creating powershell project for new day"
cd $FOLDER

echo ""
echo "creating input text files..."
touch "sample.txt"
touch "input.txt"
echo ""

echo "creating starter cs file"
sh ../boilerPlate.sh $FOLDER

echo "done";