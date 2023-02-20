set -e

if [ -z "$1" ]
  then
    echo "Please provide a solution directory day number as first argument"
    echo ""
    echo "for example:  sh solve.sh (1)"
    echo ""
    exit 1
fi

if [ -z "$2" ]
  then
    echo "Please provide an input file name to solve"
    echo ""
    echo "for example:  sh solve.sh 1 input"
    echo "for example:  sh solve.sh 1 sample"
    echo ""
    exit 1
fi

DAYDIR="Day$1"

FILE="$2.txt"

echo "solving against this input file: $FILE"

echo "solving file in folder $DAYDIR..."

cd "$DAYDIR"

echo "printing solution..."

powershell -NoProfile -File "Day$1.ps1" $1 $2

echo ""
echo "done"