import {is_email, isEmpty} from '../components/util'

describe("Utility functions", ()=>{
	test('test email validations', ()=>{
		expect(is_email("test@")).toEqual(false);
		expect(is_email("test@test.com")).toEqual(true);
	});

	test('test empty string validations', ()=>{
		expect(isEmpty(" ")).toEqual(true);
		expect(isEmpty("real content")).toEqual(false);
	});
	
})