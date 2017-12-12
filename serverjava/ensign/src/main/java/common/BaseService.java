package common;

import ensign.service.EnsignService;

public class BaseService {

	protected EnsignService ensignService;

	public EnsignService getEnsignService() {
		return ensignService;
	}

	public void setEnsignService(EnsignService ensignService) {
		this.ensignService = ensignService;
	}
	
}
