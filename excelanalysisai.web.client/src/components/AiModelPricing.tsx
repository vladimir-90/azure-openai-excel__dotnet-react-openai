import type { AIModelDto } from '../services/generation-settings.service';

interface AiModelPricingProps {
	selectedModel: AIModelDto | undefined;
}

function AiModelPricing({ selectedModel }: AiModelPricingProps) {
	const formatPrice = (price: number): string => {
		return `$${price.toFixed(2)}`;
	};

	if (!selectedModel) {
		return null;
	}

	return (
		<div className="mt-2 p-2 bg-light rounded border">
			<div className="row text-center align-items-center">
				<div className="col-3">
					<small className="text-muted fw-semibold">💰 Pricing</small>
					<br />
					<small className="text-muted">per 1M tokens</small>
				</div>
				<div className="col-3">
					<div className="text-success fw-bold">
						{formatPrice(selectedModel.pricing.input)}
					</div>
					<small className="text-muted">Input</small>
				</div>
				<div className="col-3">
					<div className="text-info fw-bold">
						{formatPrice(selectedModel.pricing.cachedInput)}
					</div>
					<small className="text-muted">Cached</small>
				</div>
				<div className="col-3">
					<div className="text-warning fw-bold">
						{formatPrice(selectedModel.pricing.output)}
					</div>
					<small className="text-muted">Output</small>
				</div>
			</div>
		</div>
	);
}

export default AiModelPricing;
