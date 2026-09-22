import type { ApiToken, ApiTokenCreated } from "../models";
import { axiosInstance } from "../utils/axiosInstance";

const domain = "admin/api-tokens";

export const getApiTokensCall = (): Promise<ApiToken[]> => {
	return axiosInstance.get<ApiToken[]>(domain).then((response) => {
		return response.data;
	});
};

export const createApiTokenCall = (name: string): Promise<ApiTokenCreated> => {
	return axiosInstance.post<ApiTokenCreated>(domain, { name }).then((response) => {
		return response.data;
	});
};

export const revokeApiTokenCall = (id: number): Promise<void> => {
	return axiosInstance.delete(`${domain}/${id}`);
};
