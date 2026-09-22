import { QUERY_KEYS } from "@/models";
import { getApiTokensCall } from "../apiTokenService";
import { createListQuery } from "./queryFactory";

export const getApiTokens = createListQuery(QUERY_KEYS.apiTokens, getApiTokensCall);
