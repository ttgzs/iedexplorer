========================  LICENSES ========================
/*
 *  Copyright (C) 2013 Pavel Charvat
 * 
 *  This file is part of IEDExplorer.
 *
 *  IEDExplorer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  IEDExplorer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with IEDExplorer.  If not, see <http://www.gnu.org/licenses/>.
 */

IEDExplorer uses Binary Notes ASN.1 software library:

/*
* Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
* 
* Licensed under the LGPL, Version 2 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
* 
*      http://www.gnu.org/copyleft/lgpl.html
* 
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
* With any your questions welcome to my e-mail 
* or blog at http://abdulla-a.blogspot.com.
*/


========================  IEDExplorer ========================


The IEDExplorer project has been created in my free time in order to learn the IEC61850 protocol.
The result is the only (as I have seen over Internet) pure .net managed MMS stack (not complete yet)
and a very basic IEC61850 MMS client implementation.

The code is NOT in production quality, written by a non-professional programmer:
- it completely lacks tests
- it uses Binary Notes implementation of MMS ASN.1 encoding/decoding, which is relatively
  slow in the C# flavour, thus not really suitable for a serious usage
- it has a simplistic UI and UI bindings (needs some refactoring)
- IEC61850 is modelled in a very simple manner, and model is incomplete (refactoring needed)
- the MMS client side only has been implemented

On the other side:
- it works (at least for me on some test setups)
- it has already helped me in some situations in debugging 61850 communications
- it has a big potential for improvement
- it can be a free alternative to some commercial test utilities
- it runs under Windows and under Linux/Mono

The project is looking for enthusiastic programmers who want to explore the mysterious
world of IEC61850 protocol.

Rev 0.1 2013/05/29
The first release 0.1 is an "as is" copy of the last development.

Rev 0.2 2013/11/01
The second release 0.2 : some enhacements in using DNS results/IP address. Most recent IP list.

Rev 0.3 2014/03/19
Some changes to make libiec61850 based server communication working.

Rev 0.4 2014/03/21
Reading variables from libiec61850 server working. Accepting reports without variable references.

Rev 0.5 2014/10/24
RP blocks have own tree.
Added NodeRP.cs
Boolean sent with 0x01.
Commands as a structure - working prototype for single commands direct.
Logging level combo.
Inspecting ied without defined lists possible, for example some libiec61850 samples.
Support for reading Arrays.

Rev 0.6 2014/11/04
New UI - begin with new libraries. Command Dialog - continue

Rev 0.7
Packet capture finished. New UI.

Rev 0.71 2014/12/07
SCL File View added thanks to Joel Kaiser.

Rev 0.72 2015/01/27
ISO layers are fully implemented now - ported from libiec61850. Thanks to the original author Michael Zillgith.
It is possible to set parameters for ISO COTP, Session and Presentation layers.
SCL View enhanced, resp. added some features from Joel Kaiser's version.

Rev 0.73 2015/02/17
Minor debug release / File Directory dialogs

Rev 0.74 2015/04/10
Minor debug release / ISO Session initialization

Rev 0.75 2015/10/21
* ISO COTP implemented sending of long data
* Report inclusion string - improved reading of reports
* New RCB Activation Dialog with Right/click on (U)RCB
* UI insulation (some internal refactoring)
* Support for buffered reports
* Reading of values with variable list
* File handling - icons
* Auto update for data window
* Delete NVL action - bugfix
* Enabled concurrent queries - InvokeId dictionary transactions queue
* Multithreading enabled for Write
* LogView context menu re-enabled (victim of internal refactoring)

Rev 0.76 2015/11/11
* Autoupdate - removed bug when reading higher level nodes (IED, LD, LN)
* Better readibility of octet_string values (converted to hex)
* Save Data button for IED Data view - save data view incl. values to TXT file for reference

Rev 0.77 2016/01/18
* SCSM_MMS: InvokeId - autoPurge (internal functionality)
  Stop Autorefresh (cyclic read) on unsuitable nodes (mostly in IEC View)
* Saving CFG - split dialog, saveModel methods.
  Bug correction - threading issue in Scsm_MMS_Worker.cs. Seen on the VMs.
  Iec tree member nodes marked
* Saving libiec61850 model file (CFG) - the feature is not yet fully finished
* IEC structure view - tree.
* reading binary_time + forgotten icon
* Refresh data button in IED data view
* Short fix DA/DO Type
* small modification in TCP/RW
* IEC data model - some first ideas
* Collapse/Expand in tree. Some more DA/DO logic.

Rev 0.77b 2016/02/04
(Service release)
* Writing of unsigned and integer values
* Test XML DOM parser for SCL Parser and View
* SCL Parser and View - more IEDs from SCD can be read in. Some improvement in parser, parser made non-static
* Small correction in variables precedence in RCB activation dialog. Icon for EXE made explicit.


Rev 0.78 experimental SCLServer release 2016/04/23
This release is an experimental release with a new feature:
IEC61850 SERVER representing data from a loaded SCL file = SCL Server
SCL Server uses libiec61850.dll and its .net extension libiec61850dotnet.dll
Both libraries are embedded in the .exe
How to use the feature:
Load a valid SCL file.
SCL Server can be run by a right-click on the IED name in the SCL tree view, select Run SCLServer
Values in the SCL Server can be changed manually only a.t.m. - right click in the SCL View Window
Changed values should be visible for all connected IEC61850 clients via reads and reporting.
GOOSE messaging and sampled values are not implemented.

SCLServer is in ALPHA experimental state!!!
SCLServer may be separated into an independent exe in the future!!!

Shortened commit review:
* SCLParserDOM2.cs - new review of SCL paresr
* SCL server enhancements, debugging - writing values
* Editing values in SCL Server model.
* Inclusive reporting with configured TrgOps.
* Tcp - disconnect recogniition
* SCLServer - initial values
* SCLView - changing values (manually)
* initial values from SCL
* SCL Arrays handling
* sub data objects from SCL
* CSL Parser - SDOs, arrays
* Enhancing SCL Server and SCL Parser
* SCLServer, SCLParserDOM.cs - unfinished. BERDecoder change - Vadim Evseev.
* SCLServer
* IEC61850.Server
* Embedding iec61850.dll and iec61850dotnet.dll. Test client in SclServer.cs.
* SCL view: DOs to LNs, icons
* SCL read into iec structure
* Watch View initial
* AutoUpdateStoppedDialog.cs
* SCLParserDOM.cs
* Viewing SCL files - SCL View
* Handling of dynamic DataSets finished

Rev 0.79 experimental SCLServer & GOOSE release 2017/03/05
This release is an experimental release with a new feature:
GOOSE functions.

* Integration of Mireks features:
- Goose Sender + Data Editor
- Goose Explorer
- Polling list
- Reports list (still ongoing)
Features are mostly UNTESTED.
- Env optimizations

* Poll View Renamed and made permanent without affecting application closing
* Some changes for resding files, a choice not to read data model values on startup
* Purge Mireks commits - delete binary files
* Correction to sending command from contextmenu on Data node
* + Goose explorer
+ Goose sender
+ Report event viewer
+ PoolViewer with Drag'N'Drop
* Workaround for initial READs

Service release 0.79b 2017/05/09
- reading SCL files
- Owner RCB field readable
- more TCP errors catched
