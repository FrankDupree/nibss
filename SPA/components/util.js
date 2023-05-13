export function is_email(email){      
	var emailReg = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
	return emailReg.test(email); 
} 

export function isEmpty(str){
    if (!str.replace(/\s/g, '').length) {
        return true;
    }
    return false;
}

export function hasNumber(myString) {
  return /\d/.test(myString);
}
