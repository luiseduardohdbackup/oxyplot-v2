#!/bin/sh

# Tools
MDTOOL="/Applications/Xamarin Studio.app/Contents/MacOS/mdtool"
UVNTOOL="../Tools/Lynx/UpdateVersionNumbers.exe"

# Folders
SOURCE=../Source
OUTPUT=../Output

# VERSION=${VERSION:=2014.1.308}

# Run the tool that updates the version numbers in all AssemblyInfo.cs files
echo "Updating version numbers"
mono $UVNTOOL /VersionFromNuGet=OxyPlot.Core /Dependency=OxyPlot.Core /Directory=.. > build-update.log
if [ $? -ne 0 ]; then 
	echo "  FAILED!"
fi

# Clean the output folder
rm -rf $OUTPUT

echo "Building for Xamarin.iOS"
# Build OxyPlot. The output will be created in the $OUTPUT folder.
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinIOS.sln > build-ios.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi

echo "Building for Xamarin.Android"
"$MDTOOL" build "--configuration:Release" $SOURCE/OxyPlot.XamarinAndroid.sln > build-android.log
if [ $? -ne 0 ]; then 
	echo "  FAILED"
else 
	echo "  OK"
fi