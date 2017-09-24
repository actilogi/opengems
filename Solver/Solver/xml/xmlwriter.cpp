// XML_Writer.cpp : 
//
#include "xmlwriter.h"

XML_Writer::XML_Writer(std::string fileName , std::string version , std::string encoding )
{
    m_XmlFile = fileName;
	m_version = version ;
	m_encoding = encoding ;
    m_vectAttrData.clear();

    fp = NULL;
    m_iLevel = 0;
    errno_t err = fopen_s( &fp, m_XmlFile.c_str(),"w");
    if( err != 0 )
    {
		fp = NULL ;
		std::cout<<"Unable to open output file";
		return;
  	}
	else
	{
		fprintf(fp,"<?xml version=\"%s\" encoding=\"%s\" \?>", m_version.c_str(), m_encoding.c_str() );
	}
}

XML_Writer::XML_Writer( )
{
    m_XmlFile = "";
    m_vectAttrData.clear();

    fp = NULL;
    m_iLevel = 0;
}

XML_Writer::~XML_Writer()
{
    if( fp != NULL)
	{
        fclose(fp);
		fp = NULL ;
	}
    m_vectAttrData.clear();
}

bool XML_Writer::openFile( std::string fileName , std::string version , std::string encoding )
{
    m_XmlFile = fileName;
	m_version = version ;
	m_encoding = encoding ;

    fp = NULL;
    errno_t err = fopen_s( &fp, m_XmlFile.c_str(),"w");
    if( err != 0 )
    {
		fp = NULL ;
		std::cout<<"Unable to open output file";
		return false ;
  	}
	else
	{
		fprintf(fp,"<?xml version=\"%s\" encoding=\"%s\" \?>", m_version.c_str(), m_encoding.c_str() );
	}

	return true ;
}


void XML_Writer::Createtag(std::string sTag)
{
	fprintf(fp,"\n");

	//Indent properly
	for(int iTmp = 0 ; iTmp < m_iLevel ; iTmp++ )
		fprintf(fp,"\t");

	fprintf(fp,"<%s",sTag.c_str() ) ;
	
	//Add Attributes
	while( 0 < m_vectAttrData.size() / 2 )
	{
		std::string sTmp = m_vectAttrData.back() ;
		fprintf(fp," %s=", sTmp.c_str()) ;
	
		m_vectAttrData.pop_back();
		sTmp = m_vectAttrData.back();
		fprintf(fp,"\"%s\"", sTmp.c_str()) ;
		m_vectAttrData.pop_back() ;
	}

	m_vectAttrData.clear();
	fprintf(fp,">");
	m_sTagStack.push(sTag);
	m_iLevel++;

}

void XML_Writer::CloseLasttag()
{
	fprintf(fp,"\n");
	m_iLevel--;

    //Indent properly
	for(int iTmp = 0 ; iTmp < m_iLevel ; iTmp++ )
		fprintf(fp,"\t");

	fprintf(fp,"</%s>",m_sTagStack.top().c_str());
	m_sTagStack.pop();//pop out the last tag

	return;
}

void XML_Writer::CloseSingletag()
{
	fseek( fp, -1, SEEK_CUR ) ;
	fprintf(fp," />");
	m_iLevel--;
    
	//Indent properly
	for(int iTmp = 0 ; iTmp < m_iLevel ; iTmp++ )
		fprintf(fp,"\t");

	//fprintf(fp,"</%s>",m_sTagStack.top().c_str());
	m_sTagStack.pop();//pop out the last tag
	return;
}


void XML_Writer::CloseAlltags()
{
	while(m_sTagStack.size() != 0)
	{
	   fprintf(fp,"\n");
	   m_iLevel--;
        //Indent properly
	   for(int iTmp =0;iTmp<m_iLevel;iTmp++)
	       fprintf(fp,"\t");
	   fprintf(fp,"</%s>",m_sTagStack.top().c_str());
	   m_sTagStack.pop();//pop out the last tag
    }
	return;
}
void XML_Writer::CreateChild(std::string sTag,std::string sValue)
{
	fprintf(fp,"\n");
	//Indent properly
	for(int iTmp = 0 ; iTmp < m_iLevel ; iTmp++ )
		fprintf(fp,"\t");

	fprintf(fp,"<%s",sTag.c_str());
	
	//Add Attributes
	while( 0 < m_vectAttrData.size() / 2 )
	{
		std::string sTmp = m_vectAttrData.back();
		fprintf(fp," %s=", sTmp.c_str());

		m_vectAttrData.pop_back();
		sTmp = m_vectAttrData.back();
		fprintf(fp,"\"%s\"", sTmp.c_str());
		m_vectAttrData.pop_back();
	}
	m_vectAttrData.clear();
	//add value and close tag
	fprintf(fp,">%s</%s>",sValue.c_str(),sTag.c_str());
}

void XML_Writer::AddAtributes(std::string sKey, std::string sVal)
{
	m_vectAttrData.push_back(sVal);
	m_vectAttrData.push_back(sKey);
}


void XML_Writer::AddComment(std::string sComment)
{
	fprintf(fp,"\n");
	//Indent properly
	for(int iTmp =0;iTmp<m_iLevel;iTmp++)
		fprintf(fp,"\t");
	fprintf(fp,"<!--%s-->",sComment.c_str());
}
