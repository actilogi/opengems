/*
 OpenGEMS is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 OpenGEMS is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

  Copyright 2007 by Computer and Communication Unlimited
*/

#include "FDTD_Common_Func.h"
#include "Global_variable.h"
#include "GEMS_Constant.h"


#include <vector>


float Excitation_Source( int n , float delay , float *pulse, int pulseLen, float Dt ) 
{
	int nn ;

	float delta,excitation_value ;

	nn = int( delay / Dt);
	delta = ( delay - nn * Dt ) / Dt ;


	if ( ( nn + n ) >= pulseLen ) 
	{
		excitation_value = 0.0 ;
	} 
	else 
	{
		if (nn >= n )
		{
			excitation_value = 0.0 ;
		} 
		else 
		{
			if( n >= pulseLen )
				excitation_value = 0.0 ;
			else
			{
				excitation_value = pulse[n - nn] * (1 - delta) + pulse[n - nn - 1] * delta ;
			}
		}
	}

	return excitation_value ;
}



void Global2Local( int xs, int ys, int zs, int &xt, int &yt, int &zt , DomainIndex localDomainIndex , int boundaryLayerNum[6] )  
{
	xt = xs - localDomainIndex.startX + 1 + boundaryLayerNum[XMIN] ;
	yt = ys - localDomainIndex.startY + 1 + boundaryLayerNum[YMIN] ;
	zt = zs - localDomainIndex.startZ + 1 + boundaryLayerNum[ZMIN] ;
}


void Local2Global( int xl, int yl, int zl, int &xg, int &yg, int &zg , DomainIndex localDomainIndex , int boundaryLayerNum[6] )  
{
	xg = xl + localDomainIndex.startX - 1 - boundaryLayerNum[XMIN] ;;
	yg = yl + localDomainIndex.startY - 1 - boundaryLayerNum[YMIN] ;;
	zg = zl + localDomainIndex.startZ - 1 - boundaryLayerNum[ZMIN] ;;

}

int indexOfList( std::vector<Point> &vlist, Point &vp )
{
	return -1;

	int i, n = static_cast< int > ( vlist.size() ) ;		
	for ( i = 0; i < n; i ++ ) {
		if ( ( vp.x == vlist[i].x ) && ( vp.y == vlist[i].y ) && ( vp.z == vlist[i].z ) ) return i;
	}
	
	return -1;
}
void OutputSigma( CMaterial ****eMatType , int *Index_E_Boundary, int boundaryLayerNum[6],
				 float *X_Grid, float *Y_Grid, float *Z_Grid , float *Dx_Grid, float *Dy_Grid, float *Dz_Grid , 
				 float *X_Grid_Global, float *Y_Grid_Global, float *Z_Grid_Global ,
				 int *Nx_Local2Global, int *Ny_Local2Global, int *Nz_Local2Global,
				 Point domainRef , float unit )
{

	FILE *fp ;

	char filename[512] ;
	char index[5] ;
	int i, ix, iy, iz , j ;

	int outer_id[8] ;
	int gridline_id[4] ;
	float  gridPoint[3][4] ;

	int nGrid ;
	bool showCell ;

	int nElement ;

	// Delete same vector
	Point vp;
	std::vector<Point> vlist[6];
	std::vector<int> flist[6];
	int ic, idx;
	
	// assign file name
	strcpy_s( filename, "." ) ;
	//itoa( idSolver, index, 10) ;
	sprintf_s( index,"%d", idSolver ) ;
	strcat_s( filename, "/sigma_disp") ; //.sig") ;
	strcat_s( filename, index ) ;
	strcat_s( filename, ".sig") ;

	nElement = 1 ;
	// open file 
	errno_t err = fopen_s( &fp, filename, "wb" ) ;
	if( err != 0 )
	{
		return  ;
	}


	float startx , starty, startz ;
	int gx1, gy1, gz1 ;
	gx1 = Nx_Local2Global[Index_E_Boundary[XMIN]] ;  
	gy1 = Ny_Local2Global[Index_E_Boundary[YMIN]] ;  
	gz1 = Nz_Local2Global[Index_E_Boundary[ZMIN]] ;  

	if( gx1 >= 0 )
		startx = X_Grid_Global[gx1] ;
	else
		startx = -( X_Grid_Global[1] - X_Grid_Global[0] ) * boundaryLayerNum[XMIN];

	if( gy1 >= 0 )
		starty = Y_Grid_Global[gy1] ;
	else
		starty = -( Y_Grid_Global[1] - Y_Grid_Global[0] ) * boundaryLayerNum[YMIN];

	if( gz1 >= 0 )
		startz = Z_Grid_Global[gz1] ;
	else
		startz = -( Z_Grid_Global[1] - Z_Grid_Global[0] ) * boundaryLayerNum[ZMIN];

	// grid cells in the X-Y plane
	for( ix = Index_E_Boundary[XMIN] ; ix < Index_E_Boundary[XMAX]; ix++ )
	{
		for( iy = Index_E_Boundary[YMIN] ; iy < Index_E_Boundary[YMAX]; iy++ )
		{
			for( iz = Index_E_Boundary[ZMIN] ; iz <= Index_E_Boundary[ZMAX]; iz++ )
			{
				memset( outer_id, 0, 8 * sizeof( int ) ) ;	
				if ( ( eMatType[ix][iy][iz]->Sig_x < 0 ) || ( eMatType[ix][iy][iz]->Sig_y < 0 ) || ( eMatType[ix][iy][iz]->Sig_z < 0 ) ) 
				{
					return ;
				}
			
				if ( iz > Index_E_Boundary[ZMIN] ) 
				{

					if ( eMatType[ix][iy][iz-1]->Sig_x > 0.01 )		outer_id[0] = 1 ;
					if ( eMatType[ix+1][iy][iz-1]->Sig_y > 0.01 )   outer_id[1] = 1 ;
					if ( eMatType[ix][iy+1][iz-1]->Sig_x > 0.01 )   outer_id[2] = 1 ;
					if ( eMatType[ix][iy][iz-1]->Sig_y > 0.01 )		outer_id[3] = 1 ;

				}

				if ( iz < Index_E_Boundary[ZMAX] ) 
				{
					if ( eMatType[ix][iy][iz+1]->Sig_x > 0.01 )		outer_id[4] = 1 ;
					if ( eMatType[ix+1][iy][iz+1]->Sig_y > 0.01 )   outer_id[5] = 1 ;
					if ( eMatType[ix][iy+1][iz+1]->Sig_x > 0.01 )   outer_id[6] = 1 ;
					if ( eMatType[ix][iy][iz+1]->Sig_y > 0.01 )		outer_id[7] = 1 ;
				}
			
				memset( gridline_id, 0, 4 * sizeof( int ) ) ;
				if ( eMatType[ix][iy][iz]->Sig_x > 0.01 )		gridline_id[0] = 1 ;
				if ( eMatType[ix+1][iy][iz]->Sig_y > 0.01 )		gridline_id[1] = 1 ;
				if ( eMatType[ix][iy+1][iz]->Sig_x > 0.01 )		gridline_id[2] = 1 ;
				if ( eMatType[ix][iy][iz]->Sig_y > 0.01 )		gridline_id[3] = 1 ;

				nGrid = 0 ;
				showCell = false ;
				for( i = 0 ; i < 8 ; i++ )
					if( outer_id[i] == 0 ) showCell = true ;

				for( i = 0 ; i < 4 ; i++ )
					if( gridline_id[i] > 0 ) nGrid++ ;

				if ( nGrid < 3 ) showCell = false ;
				
				//! get patches on the mesh cell
				if ( showCell ) 
				{
					nElement++ ;
					
					gridPoint[0][0] = X_Grid[ix] + startx ; 
					gridPoint[1][0] = Y_Grid[iy] + starty ; 
					gridPoint[2][0] = Z_Grid[iz] + startz ;
					gridPoint[0][1] = X_Grid[ix+1] + startx ;
					gridPoint[1][1] = Y_Grid[iy] + starty ;
					gridPoint[2][1] = Z_Grid[iz] + startz ;
					gridPoint[0][2] = X_Grid[ix+1] + startx ;
					gridPoint[1][2] = Y_Grid[iy+1] + starty ;
					gridPoint[2][2] = Z_Grid[iz] + startz ;
					gridPoint[0][3] = X_Grid[ix] + startx ;
					gridPoint[1][3] = Y_Grid[iy+1] + starty ; 
					gridPoint[2][3] = Z_Grid[iz] + startz ;

					if ( nGrid == 4 ) 
					{
						ic = 0;
					} 
					else 
					{
						ic = 3;
					}

					flist[ic].push_back( 4 );

					for( j = 0 ; j < 4 ; j++ )
					{
						vp.x = gridPoint[0][j];
						vp.y = gridPoint[1][j];
						vp.z = gridPoint[2][j];

						
						idx = indexOfList( vlist[ic], vp );
						if ( idx < 0 ) 
						{
							vlist[ic].push_back( vp );
							idx = static_cast<int>( vlist[ic].size() ) - 1;
						}
						flist[ic].push_back( idx );
					}					

				}	// end of if ( showCell )
			
			}// end of for( iz = BoundaryLayerNum[ZMIN] + Index_E_Boundary[ZMIN] ; iy < Index_E_Boundary[ZMAX] - BoundaryLayerNum[ZMAX] ; iz++ )
		}
	}

	// grid cells in the X-Z plane
	for( ix = Index_E_Boundary[XMIN] ; ix < Index_E_Boundary[XMAX]; ix++ )
	{
		for( iy = Index_E_Boundary[YMIN] ; iy <= Index_E_Boundary[YMAX]; iy++ )
		{
			for( iz = Index_E_Boundary[ZMIN] ; iz < Index_E_Boundary[ZMAX]; iz++ )
			{
				memset( outer_id, 0, 8 * sizeof( int ) ) ;
				
				if ( iy > Index_E_Boundary[YMIN] ) 
				{
					if ( eMatType[ix][iy-1][iz]->Sig_x > 0.01 )		outer_id[0] = 1 ;
					if ( eMatType[ix+1][iy-1][iz]->Sig_z > 0.01 )   outer_id[1] = 1 ;
					if ( eMatType[ix][iy-1][iz+1]->Sig_x > 0.01 )   outer_id[2] = 1 ;
					if ( eMatType[ix][iy-1][iz]->Sig_z > 0.01 )		outer_id[3] = 1 ;

				}

				if ( iy < Index_E_Boundary[YMAX] ) 
				{
					if ( eMatType[ix][iy+1][iz]->Sig_x > 0.01 )		outer_id[4] = 1 ;
					if ( eMatType[ix+1][iy+1][iz]->Sig_z > 0.01 )   outer_id[5] = 1 ;
					if ( eMatType[ix][iy+1][iz+1]->Sig_x > 0.01 )   outer_id[6] = 1 ;
					if ( eMatType[ix][iy+1][iz]->Sig_z > 0.01 )		outer_id[7] = 1 ;
				}
			
				memset( gridline_id, 0, 4 * sizeof( int ) ) ;
				if ( eMatType[ix][iy][iz]->Sig_x > 0.01 )		gridline_id[0] = 1 ;
				if ( eMatType[ix+1][iy][iz]->Sig_z > 0.01 )		gridline_id[1] = 1 ;
				if ( eMatType[ix][iy][iz+1]->Sig_x > 0.01 )		gridline_id[2] = 1 ;
				if ( eMatType[ix][iy][iz]->Sig_z > 0.01 )		gridline_id[3] = 1 ;

				nGrid = 0 ;
				showCell = false ;
				for( i = 0 ; i < 8 ; i++ )
					if( outer_id[i] == 0 ) showCell = true ;

				for( i = 0 ; i < 4 ; i++ )
					if( gridline_id[i] > 0 ) nGrid = nGrid + 1 ;

				if ( nGrid < 3 ) showCell = false ;
				
				//! get patches on the mesh cell
				if ( showCell ) 
				{
					nElement++ ;
					
					gridPoint[0][0] = X_Grid[ix] + startx ; 
					gridPoint[1][0] = Y_Grid[iy] + starty ; 
					gridPoint[2][0] = Z_Grid[iz] + startz ;
					gridPoint[0][1] = X_Grid[ix+1] + startx ;
					gridPoint[1][1] = Y_Grid[iy] + starty ;
					gridPoint[2][1] = Z_Grid[iz] + startz ;
					gridPoint[0][2] = X_Grid[ix+1] + startx ;
					gridPoint[1][2] = Y_Grid[iy] + starty ;
					gridPoint[2][2] = Z_Grid[iz+1] + startz ;
					gridPoint[0][3] = X_Grid[ix] + startx ;
					gridPoint[1][3] = Y_Grid[iy] + starty ; 
					gridPoint[2][3] = Z_Grid[iz+1] + startz ;

					if ( nGrid == 4 ) 
					{
						ic = 1;
					} 
					else 
					{
						ic = 4;
					}

					flist[ic].push_back( 4 );

					for( j = 0 ; j < 4 ; j++ )
					{
						vp.x = gridPoint[0][j];
						vp.y = gridPoint[1][j];
						vp.z = gridPoint[2][j];

						
						idx = indexOfList( vlist[ic], vp );
						if ( idx < 0 ) 
						{
							vlist[ic].push_back( vp );
							idx = static_cast<int>( vlist[ic].size()) - 1;
						}
						flist[ic].push_back( idx );
					}					

				}	// end of if ( showCell )
			
			}// end of for( iz = BoundaryLayerNum[ZMIN] + Index_E_Boundary[ZMIN] ; iy < Index_E_Boundary[ZMAX] - BoundaryLayerNum[ZMAX] ; iz++ )
		}
	}

	// grid cells in the Y-Z plane
	for( ix = Index_E_Boundary[XMIN] ; ix <= Index_E_Boundary[XMAX]; ix++ )
	{
		for( iy = Index_E_Boundary[YMIN] ; iy < Index_E_Boundary[YMAX]; iy++ )
		{
			for( iz = Index_E_Boundary[ZMIN] ; iz < Index_E_Boundary[ZMAX]; iz++ )
			{
				memset( outer_id, 0, 8 * sizeof( int ) ) ;
				
				if ( ix > Index_E_Boundary[XMIN] ) 
				{
					if ( eMatType[ix-1][iy][iz]->Sig_y > 0.01 )		outer_id[0] = 1 ;
					if ( eMatType[ix-1][iy+1][iz]->Sig_z > 0.01 )   outer_id[1] = 1 ;
					if ( eMatType[ix-1][iy][iz+1]->Sig_y > 0.01 )   outer_id[2] = 1 ;
					if ( eMatType[ix-1][iy][iz]->Sig_z > 0.01 )		outer_id[3] = 1 ;

				}

				if ( ix < Index_E_Boundary[XMAX] ) 
				{
					if ( eMatType[ix+1][iy][iz]->Sig_y > 0.01 )		outer_id[4] = 1 ;
					if ( eMatType[ix+1][iy+1][iz]->Sig_z > 0.01 )   outer_id[5] = 1 ;
					if ( eMatType[ix+1][iy][iz+1]->Sig_y > 0.01 )   outer_id[6] = 1 ;
					if ( eMatType[ix+1][iy][iz]->Sig_z > 0.01 )		outer_id[7] = 1 ;
				}
			
				memset( gridline_id, 0, 4 * sizeof( int ) ) ;
				if ( eMatType[ix][iy][iz]->Sig_y > 0.01 )		gridline_id[0] = 1 ;
				if ( eMatType[ix][iy+1][iz]->Sig_z > 0.01 )		gridline_id[1] = 1 ;
				if ( eMatType[ix][iy][iz+1]->Sig_y > 0.01 )		gridline_id[2] = 1 ;
				if ( eMatType[ix][iy][iz]->Sig_z > 0.01 )		gridline_id[3] = 1 ;

				nGrid = 0 ;
				showCell = false ;
				for( i = 0 ; i < 8 ; i++ )
					if( outer_id[i] == 0 ) showCell = true ;

				for( i = 0 ; i < 4 ; i++ )
					if( gridline_id[i] > 0 ) nGrid = nGrid + 1 ;

				if ( nGrid < 3 ) showCell = false ;
				
				//! get patches on the mesh cell
				if ( showCell ) 
				{
					nElement = nElement + 1 ;
					
					gridPoint[0][0] = X_Grid[ix] + startx ; 
					gridPoint[1][0] = Y_Grid[iy] + starty ; 
					gridPoint[2][0] = Z_Grid[iz] + startz ;
					gridPoint[0][1] = X_Grid[ix] + startx ;
					gridPoint[1][1] = Y_Grid[iy+1] + starty ;
					gridPoint[2][1] = Z_Grid[iz] + startz ;
					gridPoint[0][2] = X_Grid[ix] + startx ;
					gridPoint[1][2] = Y_Grid[iy+1] + starty ;
					gridPoint[2][2] = Z_Grid[iz+1] + startz ;
					gridPoint[0][3] = X_Grid[ix] + startx ;
					gridPoint[1][3] = Y_Grid[iy] + starty ; 
					gridPoint[2][3] = Z_Grid[iz+1] + startz ;

					if ( nGrid == 4 ) 
					{
						ic = 2;
					} 
					else 
					{
						ic = 5;
					}

					flist[ic].push_back( 4 );

					for( j = 0 ; j < 4 ; j++ )
					{
						vp.x = gridPoint[0][j];
						vp.y = gridPoint[1][j];
						vp.z = gridPoint[2][j];

						
						idx = indexOfList( vlist[ic], vp );
						if ( idx < 0 ) 
						{
							vlist[ic].push_back( vp );
							idx = static_cast<int>( vlist[ic].size() ) - 1;
						}
						flist[ic].push_back( idx );
					}

				}	// end of if ( showCell )
			
			}// end of for( iz = BoundaryLayerNum[ZMIN] + Index_E_Boundary[ZMIN] ; iy < Index_E_Boundary[ZMAX] - BoundaryLayerNum[ZMAX] ; iz++ )
		}
	}
	
	char char_out[4] ;
	char *p ;
	int n ;
	float out ;
	for ( ic = 0; ic < 6; ic ++ ) 
	{		

		n = static_cast<int>( vlist[ic].size() ) ;
		p = (char *)( &n ) ;
		for( j = 0 ; j < 4 ; j++ )
			char_out[j] = p[j] ;

		for( j = 0 ; j < 4 ; j++ )
			char_out[j] = p[3-j] ;
		fwrite( char_out, sizeof( char ), 4 , fp ) ;

		for ( i = 0; i < n; i ++ ) 
		{
			p = (char *)( &out ) ;

			out = ( vlist[ic][i].x + domainRef.x - Dx_Grid[0] ) / unit ;
			for( j = 0 ; j < 4 ; j++ )
				char_out[j] = p[3-j] ;
			fwrite( char_out, sizeof( char ), 4 , fp ) ;

			out = ( vlist[ic][i].y + domainRef.y - Dy_Grid[0] ) / unit ;
			for( j = 0 ; j < 4 ; j++ )
				char_out[j] = p[3-j] ;
			fwrite( char_out, sizeof( char ), 4 , fp ) ;

			out = ( vlist[ic][i].z + domainRef.z - Dz_Grid[0] ) / unit ;
			for( j = 0 ; j < 4 ; j++ )
				char_out[j] = p[3-j] ;
			fwrite( char_out, sizeof( char ), 4 , fp ) ;

		}
		
		n = static_cast<int>( flist[ic].size() ) ;
		p = (char *)( &n ) ;
		for( j = 0 ; j < 4 ; j++ )
			char_out[j] = p[3-j] ;
		fwrite( char_out, sizeof( char ), 4 , fp ) ;

		int fn ;
		for ( i = 0; i < n; i ++ ) 
		{
			fn = flist[ic][i] ;
			p = (char *)( &fn ) ;
			for( j = 0 ; j < 4 ; j++ )
				char_out[j] = p[3-j] ;
			fwrite( char_out, sizeof( char ), 4 , fp ) ;

		}
		
	}
	

	fclose( fp ) ;
}

