#ifndef XMLWRITER_HEADER_FILE
#define XMLWRITER_HEADER_FILE

#include <iostream>
#include "string"
#include<vector>
#include <stack>


typedef std::stack<std::string> StackStrings;


// This is a simple class used to write xml format file

class XML_Writer{
public:
	XML_Writer( );
	XML_Writer( std::string fileName , std::string version = "1.0", std::string encoding = "ISO-8859-1" );
	~XML_Writer();

	bool openFile( std::string fileName , std::string version = "1.0", std::string encoding = "ISO-8859-1" ) ;

	void CreateChild(std::string sTag,std::string sValue);
	void Createtag(std::string sTag);
	
	void CloseLasttag();
	void CloseSingletag();

	void CloseAlltags();
	void AddAtributes(std::string sAttrName, std::string sAttrvalue);
	void AddComment(std::string sComment);
private:
	std::string m_XmlFile;
	std::vector<std::string> m_vectAttrData;
	FILE *fp;
	int m_iLevel;
	StackStrings m_sTagStack;

	std::string m_version ;
	std::string m_encoding ;
};

#endif // xmlwriter_h


