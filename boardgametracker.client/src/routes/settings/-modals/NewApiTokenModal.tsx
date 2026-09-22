import { useState } from "react";
import { useTranslation } from "react-i18next";
import BgtButton from "@/components/BgtButton/BgtButton";
import {
	BgtDialog,
	BgtDialogClose,
	BgtDialogContent,
	BgtDialogDescription,
	BgtDialogTitle,
} from "@/components/BgtDialog";

interface Props {
	open: boolean;
	close: () => void;
	name: string;
	token: string;
}

export const NewApiTokenModal = ({ open, close, name, token }: Props) => {
	const { t } = useTranslation(["settings", "common"]);
	const [copied, setCopied] = useState(false);

	const handleCopy = async () => {
		await navigator.clipboard.writeText(token);
		setCopied(true);
		setTimeout(() => setCopied(false), 2000);
	};

	return (
		<BgtDialog open={open} onClose={close}>
			<BgtDialogContent>
				<BgtDialogTitle>{t("api-tokens.new-token.title")}</BgtDialogTitle>
				<BgtDialogDescription>{t("api-tokens.new-token.description", { name })}</BgtDialogDescription>
				<div className="my-4 rounded-lg bg-white/5 border border-white/10 p-4">
					<code className="text-sm font-mono text-white break-all">{token}</code>
				</div>
				<BgtDialogClose>
					<BgtButton variant="primary" onClick={handleCopy}>
						{copied ? t("api-tokens.new-token.copied") : t("api-tokens.new-token.copy")}
					</BgtButton>
					<BgtButton variant="cancel" onClick={close}>
						{t("common:close")}
					</BgtButton>
				</BgtDialogClose>
			</BgtDialogContent>
		</BgtDialog>
	);
};
