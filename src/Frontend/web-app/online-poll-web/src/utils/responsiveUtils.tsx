import React from 'react';
import {
  Theme,
  useMediaQuery,
  useTheme,
  Grid,
  GridProps,
  GridSize
} from '@mui/material';

interface ResponsiveGridProps extends Omit<GridProps, 'item'> {
  children: React.ReactNode;
  spacing?: number;
  itemProps?: {
    xs?: GridSize;
    sm?: GridSize;
    md?: GridSize;
    lg?: GridSize;
    xl?: GridSize;
  };
}

export const useResponsive = () => {
  const theme = useTheme();

  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const isTablet = useMediaQuery(theme.breakpoints.between('sm', 'md'));
  const isDesktop = useMediaQuery(theme.breakpoints.up('md'));
  const isLargeScreen = useMediaQuery(theme.breakpoints.up('lg'));
  const isExtraLargeScreen = useMediaQuery(theme.breakpoints.up('xl'));

  return {
    isMobile,
    isTablet,
    isDesktop,
    isLargeScreen,
    isExtraLargeScreen,
    breakpoint: isMobile
      ? 'xs'
      : isTablet
        ? 'sm'
        : isDesktop
          ? 'md'
          : isLargeScreen
            ? 'lg'
            : 'xl'
  };
};

export const ResponsiveGrid: React.FC<ResponsiveGridProps> = ({
  children,
  spacing = 2,
  itemProps = {},
  ...gridProps
}) => {
  const defaultItemProps = {
    xs: 12,
    sm: 6,
    md: 4,
    lg: 3,
    xl: 2,
    ...itemProps
  };

  return (
    <Grid container spacing={spacing} {...gridProps}>
      {React.Children.map(children, (child) => (
        <Grid
          item
          xs={defaultItemProps.xs}
          sm={defaultItemProps.sm}
          md={defaultItemProps.md}
          lg={defaultItemProps.lg}
          xl={defaultItemProps.xl}
        >
          {child}
        </Grid>
      ))}
    </Grid>
  );
};

export const useResponsiveValue = <T,>(
  values: {
    xs?: T;
    sm?: T;
    md?: T;
    lg?: T;
    xl?: T;
  },
  defaultValue?: T
): T | undefined => {
  const { isMobile, isTablet, isDesktop, isLargeScreen, isExtraLargeScreen } = useResponsive();

  if (isExtraLargeScreen) return values.xl ?? defaultValue;
  if (isLargeScreen) return values.lg ?? defaultValue;
  if (isDesktop) return values.md ?? defaultValue;
  if (isTablet) return values.sm ?? defaultValue;
  if (isMobile) return values.xs ?? defaultValue;

  return defaultValue;
};

export const getResponsiveFontSize = (
  baseSize: number,
  factor: number = 0.5
): number => {
  const { isMobile, isTablet, isDesktop, isLargeScreen } = useResponsive();

  if (isMobile) return baseSize - factor;
  if (isTablet) return baseSize;
  if (isDesktop) return baseSize + factor;
  if (isLargeScreen) return baseSize + (factor * 2);

  return baseSize + (factor * 3);
};

export const ResponsiveExample: React.FC = () => {
  const dynamicValue = useResponsiveValue({
    xs: 'Mobile View',
    md: 'Desktop View'
  }, 'Default View');

  const fontSize = getResponsiveFontSize(16);

  return (
    <ResponsiveGrid>
      <div style={{ fontSize }}>
        {dynamicValue}
      </div>
    </ResponsiveGrid>
  );
};

// Additional Utility: Responsive Container
export const ResponsiveContainer: React.FC<{
  children: React.ReactNode;
  maxWidth?: 'xs' | 'sm' | 'md' | 'lg' | 'xl';
}> = ({ children, maxWidth = 'md' }) => {
  const theme = useTheme();
  const { isMobile } = useResponsive();

  return (
    <div
      style={{
        maxWidth: theme.breakpoints.values[maxWidth],
        width: '100%',
        margin: '0 auto',
        padding: isMobile ? theme.spacing(2) : theme.spacing(3)
      }}
    >
      {children}
    </div>
  );
};

// Responsive Image Component
export const ResponsiveImage: React.FC<{
  src: string;
  alt: string;
  sizes?: {
    xs?: string;
    sm?: string;
    md?: string;
    lg?: string;
    xl?: string;
  };
}> = ({ src, alt, sizes = {} }) => {
  const defaultSizes = {
    xs: '100%',
    sm: '75%',
    md: '50%',
    lg: '40%',
    xl: '30%'
  };

  const mergedSizes = { ...defaultSizes, ...sizes };
  const width = useResponsiveValue(mergedSizes, '100%');

  return (
    <img
      src={src}
      alt={alt}
      style={{
        width: width,
        height: 'auto',
        display: 'block',
        margin: '0 auto'
      }}
    />
  );
};