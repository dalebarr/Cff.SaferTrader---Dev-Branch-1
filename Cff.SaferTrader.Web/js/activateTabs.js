addLoadListener(tabSwitch);

function tabSwitch() {	
	showSection(0);	
	return true;
}			
			
function showSection(id) {
	for(var i=0; i<7; i ++) {
		if(document.getElementById('tab'+i)) {
			document.getElementById('tab'+i).style.display = 'none';
			document.getElementById('tabHead'+i).className = 'inactive';							
		}						
	}

	document.getElementById('tab'+id).style.display = 'block';
	document.getElementById('tabHead'+id).className = '';
	return false;
}

